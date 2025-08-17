import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  TouchableOpacity,
  RefreshControl,
  Alert,
} from 'react-native';
import { Card } from '../components/Card';
import { apiService } from '../services/api';
import { WorkflowInstance } from '../types/api';

const WorkflowsScreen: React.FC = () => {
  const [workflows, setWorkflows] = useState<WorkflowInstance[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  const loadWorkflows = async () => {
    try {
      const response = await apiService.getWorkflows();
      if (response.success) {
        setWorkflows(response.data.data);
      } else {
        Alert.alert('Error', response.message || 'Failed to load workflows');
      }
    } catch (error) {
      console.error('Failed to load workflows:', error);
      setWorkflows([
        {
          id: '1',
          templateId: 'template1',
          templateName: 'AI Request Approval',
          status: 'active',
          progress: 65,
          startedAt: '2024-01-15T10:00:00Z',
          currentStep: 'Manager Review',
          assignedTo: 'John Manager',
          priority: 'high',
          metadata: {},
        },
        {
          id: '2',
          templateId: 'template2',
          templateName: 'Quality Assessment',
          status: 'completed',
          progress: 100,
          startedAt: '2024-01-14T09:00:00Z',
          completedAt: '2024-01-15T11:00:00Z',
          duration: 1560, // minutes
          priority: 'normal',
          metadata: {},
        },
        {
          id: '3',
          templateId: 'template3',
          templateName: 'Data Processing',
          status: 'suspended',
          progress: 30,
          startedAt: '2024-01-13T14:00:00Z',
          currentStep: 'Data Validation',
          priority: 'normal',
          metadata: {},
        },
      ]);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadWorkflows();
  }, []);

  const onRefresh = () => {
    setRefreshing(true);
    loadWorkflows();
  };

  const handleWorkflowAction = async (workflowId: string, action: string) => {
    try {
      let response;
      switch (action) {
        case 'pause':
          response = await apiService.pauseWorkflow(workflowId);
          break;
        case 'resume':
          response = await apiService.resumeWorkflow(workflowId);
          break;
        case 'cancel':
          response = await apiService.cancelWorkflow(workflowId);
          break;
        default:
          return;
      }

      if (response.success) {
        loadWorkflows();
        Alert.alert('Success', `Workflow ${action}d successfully`);
      } else {
        Alert.alert('Error', response.message || `Failed to ${action} workflow`);
      }
    } catch (error) {
      console.error(`Failed to ${action} workflow:`, error);
      Alert.alert('Error', `Failed to ${action} workflow`);
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active':
        return '#10b981';
      case 'suspended':
        return '#f59e0b';
      case 'completed':
        return '#6b7280';
      case 'terminated':
      case 'failed':
        return '#ef4444';
      default:
        return '#6b7280';
    }
  };

  const formatDuration = (minutes?: number) => {
    if (!minutes) return 'N/A';
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return hours > 0 ? `${hours}h ${mins}m` : `${mins}m`;
  };

  const renderWorkflow = ({ item }: { item: WorkflowInstance }) => (
    <Card style={styles.workflowCard}>
      <View style={styles.workflowHeader}>
        <View style={styles.workflowInfo}>
          <Text style={styles.workflowTitle}>{item.templateName}</Text>
          <Text style={styles.workflowId}>ID: {item.id}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(item.status) }]}>
          <Text style={styles.statusText}>{item.status}</Text>
        </View>
      </View>

      <View style={styles.progressContainer}>
        <Text style={styles.progressLabel}>Progress: {item.progress}%</Text>
        <View style={styles.progressBar}>
          <View
            style={[styles.progressFill, { width: `${item.progress}%` }]}
          />
        </View>
      </View>

      <View style={styles.workflowDetails}>
        <Text style={styles.detailText}>
          Started: {new Date(item.startedAt).toLocaleDateString()}
        </Text>
        {item.currentStep && (
          <Text style={styles.detailText}>Current Step: {item.currentStep}</Text>
        )}
        {item.assignedTo && (
          <Text style={styles.detailText}>Assigned to: {item.assignedTo}</Text>
        )}
        {item.duration && (
          <Text style={styles.detailText}>Duration: {formatDuration(item.duration)}</Text>
        )}
      </View>

      <View style={styles.actionButtons}>
        {item.status === 'active' && (
          <TouchableOpacity
            style={[styles.actionButton, styles.pauseButton]}
            onPress={() => handleWorkflowAction(item.id, 'pause')}
          >
            <Text style={styles.actionButtonText}>Pause</Text>
          </TouchableOpacity>
        )}
        {item.status === 'suspended' && (
          <TouchableOpacity
            style={[styles.actionButton, styles.resumeButton]}
            onPress={() => handleWorkflowAction(item.id, 'resume')}
          >
            <Text style={styles.actionButtonText}>Resume</Text>
          </TouchableOpacity>
        )}
        {(item.status === 'active' || item.status === 'suspended') && (
          <TouchableOpacity
            style={[styles.actionButton, styles.cancelButton]}
            onPress={() => handleWorkflowAction(item.id, 'cancel')}
          >
            <Text style={styles.actionButtonText}>Cancel</Text>
          </TouchableOpacity>
        )}
      </View>
    </Card>
  );

  if (loading && workflows.length === 0) {
    return (
      <View style={styles.loadingContainer}>
        <Text style={styles.loadingText}>Loading workflows...</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>Workflows</Text>
        <Text style={styles.subtitle}>Monitor and manage workflow instances</Text>
      </View>

      <FlatList
        data={workflows}
        renderItem={renderWorkflow}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.listContainer}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
      />
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
    padding: 20,
    paddingBottom: 10,
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#111827',
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 16,
    color: '#6b7280',
  },
  listContainer: {
    padding: 20,
    paddingTop: 10,
  },
  workflowCard: {
    marginBottom: 16,
  },
  workflowHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  workflowInfo: {
    flex: 1,
  },
  workflowTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#111827',
    marginBottom: 4,
  },
  workflowId: {
    fontSize: 12,
    color: '#9ca3af',
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
  },
  statusText: {
    fontSize: 12,
    fontWeight: '500',
    color: '#ffffff',
  },
  progressContainer: {
    marginBottom: 12,
  },
  progressLabel: {
    fontSize: 14,
    color: '#374151',
    marginBottom: 8,
  },
  progressBar: {
    height: 8,
    backgroundColor: '#e5e7eb',
    borderRadius: 4,
    overflow: 'hidden',
  },
  progressFill: {
    height: '100%',
    backgroundColor: '#3b82f6',
  },
  workflowDetails: {
    marginBottom: 16,
    gap: 4,
  },
  detailText: {
    fontSize: 12,
    color: '#6b7280',
  },
  actionButtons: {
    flexDirection: 'row',
    gap: 8,
  },
  actionButton: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 6,
    flex: 1,
  },
  pauseButton: {
    backgroundColor: '#f59e0b',
  },
  resumeButton: {
    backgroundColor: '#10b981',
  },
  cancelButton: {
    backgroundColor: '#ef4444',
  },
  actionButtonText: {
    fontSize: 12,
    fontWeight: '500',
    color: '#ffffff',
    textAlign: 'center',
  },
});

export default WorkflowsScreen;
