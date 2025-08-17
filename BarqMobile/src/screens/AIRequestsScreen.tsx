import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  TouchableOpacity,
  RefreshControl,
  Alert,
  Modal,
  TextInput,
} from 'react-native';
import { Card } from '../components/Card';
import { apiService } from '../services/api';
import { AIRequest, CreateAIRequestRequest, AIRequestType, Priority } from '../types/api';

const AIRequestsScreen: React.FC = () => {
  const [requests, setRequests] = useState<AIRequest[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [newRequest, setNewRequest] = useState<CreateAIRequestRequest>({
    title: '',
    description: '',
    type: 'text-generation',
    priority: 'normal',
    tags: [],
    metadata: {},
  });

  const loadRequests = async () => {
    try {
      const response = await apiService.getAIRequests();
      if (response.success) {
        setRequests(response.data.data);
      } else {
        Alert.alert('Error', response.message || 'Failed to load AI requests');
      }
    } catch (error) {
      console.error('Failed to load AI requests:', error);
      setRequests([
        {
          id: '1',
          title: 'Content Generation for Marketing',
          description: 'Generate marketing content for new product launch',
          type: 'text-generation',
          priority: 'high',
          status: 'approved',
          requesterId: 'user1',
          requesterName: 'John Doe',
          createdAt: '2024-01-15T10:00:00Z',
          updatedAt: '2024-01-15T10:00:00Z',
          dueDate: '2024-01-20T10:00:00Z',
          tags: ['marketing', 'content'],
          metadata: {},
        },
        {
          id: '2',
          title: 'Data Analysis Report',
          description: 'Analyze customer behavior data',
          type: 'data-analysis',
          priority: 'normal',
          status: 'under-review',
          requesterId: 'user2',
          requesterName: 'Jane Smith',
          createdAt: '2024-01-14T09:00:00Z',
          updatedAt: '2024-01-14T09:00:00Z',
          dueDate: '2024-01-18T09:00:00Z',
          tags: ['analytics', 'data'],
          metadata: {},
        },
      ]);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadRequests();
  }, []);

  const onRefresh = () => {
    setRefreshing(true);
    loadRequests();
  };

  const handleCreateRequest = async () => {
    if (!newRequest.title.trim() || !newRequest.description.trim()) {
      Alert.alert('Error', 'Please fill in all required fields');
      return;
    }

    try {
      const response = await apiService.createAIRequest(newRequest);
      if (response.success) {
        setShowCreateModal(false);
        setNewRequest({
          title: '',
          description: '',
          type: 'text-generation',
          priority: 'normal',
          tags: [],
          metadata: {},
        });
        loadRequests();
        Alert.alert('Success', 'AI request created successfully');
      } else {
        Alert.alert('Error', response.message || 'Failed to create AI request');
      }
    } catch (error) {
      console.error('Failed to create AI request:', error);
      Alert.alert('Error', 'Failed to create AI request');
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'approved':
        return '#10b981';
      case 'pending':
        return '#f59e0b';
      case 'in-progress':
        return '#3b82f6';
      case 'completed':
        return '#6b7280';
      case 'rejected':
        return '#ef4444';
      default:
        return '#6b7280';
    }
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'high':
      case 'critical':
        return '#ef4444';
      case 'normal':
        return '#6b7280';
      case 'low':
        return '#10b981';
      default:
        return '#6b7280';
    }
  };

  const renderRequest = ({ item }: { item: AIRequest }) => (
    <Card style={styles.requestCard}>
      <View style={styles.requestHeader}>
        <Text style={styles.requestTitle}>{item.title}</Text>
        <View style={styles.badges}>
          <View style={[styles.badge, { backgroundColor: getStatusColor(item.status) }]}>
            <Text style={styles.badgeText}>{item.status}</Text>
          </View>
          <View style={[styles.badge, { backgroundColor: getPriorityColor(item.priority) }]}>
            <Text style={styles.badgeText}>{item.priority}</Text>
          </View>
        </View>
      </View>
      <Text style={styles.requestDescription}>{item.description}</Text>
      <View style={styles.requestMeta}>
        <Text style={styles.metaText}>Type: {item.type}</Text>
        <Text style={styles.metaText}>Requester: {item.requesterName}</Text>
        {item.dueDate && (
          <Text style={styles.metaText}>
            Due: {new Date(item.dueDate).toLocaleDateString()}
          </Text>
        )}
      </View>
    </Card>
  );

  if (loading && requests.length === 0) {
    return (
      <View style={styles.loadingContainer}>
        <Text style={styles.loadingText}>Loading AI requests...</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>AI Requests</Text>
        <TouchableOpacity
          style={styles.createButton}
          onPress={() => setShowCreateModal(true)}
        >
          <Text style={styles.createButtonText}>+ New Request</Text>
        </TouchableOpacity>
      </View>

      <FlatList
        data={requests}
        renderItem={renderRequest}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.listContainer}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
      />

      <Modal
        visible={showCreateModal}
        animationType="slide"
        presentationStyle="pageSheet"
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalHeader}>
            <TouchableOpacity onPress={() => setShowCreateModal(false)}>
              <Text style={styles.cancelButton}>Cancel</Text>
            </TouchableOpacity>
            <Text style={styles.modalTitle}>New AI Request</Text>
            <TouchableOpacity onPress={handleCreateRequest}>
              <Text style={styles.saveButton}>Create</Text>
            </TouchableOpacity>
          </View>

          <View style={styles.modalContent}>
            <TextInput
              style={styles.input}
              placeholder="Request Title"
              value={newRequest.title}
              onChangeText={(text) => setNewRequest({ ...newRequest, title: text })}
            />
            <TextInput
              style={[styles.input, styles.textArea]}
              placeholder="Description"
              value={newRequest.description}
              onChangeText={(text) => setNewRequest({ ...newRequest, description: text })}
              multiline
              numberOfLines={4}
            />
          </View>
        </View>
      </Modal>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f8fafc',
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  loadingText: {
    fontSize: 16,
    color: '#6b7280',
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 20,
    paddingBottom: 10,
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#111827',
  },
  createButton: {
    backgroundColor: '#2563eb',
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 8,
  },
  createButtonText: {
    color: '#ffffff',
    fontWeight: '600',
  },
  listContainer: {
    padding: 20,
    paddingTop: 10,
  },
  requestCard: {
    marginBottom: 16,
  },
  requestHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 8,
  },
  requestTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#111827',
    flex: 1,
    marginRight: 12,
  },
  badges: {
    flexDirection: 'row',
    gap: 8,
  },
  badge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
  },
  badgeText: {
    fontSize: 12,
    fontWeight: '500',
    color: '#ffffff',
  },
  requestDescription: {
    fontSize: 14,
    color: '#6b7280',
    marginBottom: 12,
  },
  requestMeta: {
    gap: 4,
  },
  metaText: {
    fontSize: 12,
    color: '#9ca3af',
  },
  modalContainer: {
    flex: 1,
    backgroundColor: '#ffffff',
  },
  modalHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 20,
    borderBottomWidth: 1,
    borderBottomColor: '#e5e7eb',
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#111827',
  },
  cancelButton: {
    fontSize: 16,
    color: '#6b7280',
  },
  saveButton: {
    fontSize: 16,
    color: '#2563eb',
    fontWeight: '600',
  },
  modalContent: {
    padding: 20,
  },
  input: {
    borderWidth: 1,
    borderColor: '#d1d5db',
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
    marginBottom: 16,
  },
  textArea: {
    height: 100,
    textAlignVertical: 'top',
  },
});

export default AIRequestsScreen;
