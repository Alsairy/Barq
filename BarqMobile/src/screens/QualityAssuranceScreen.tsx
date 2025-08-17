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
import { QualityAssessment, CompleteQualityAssessmentRequest } from '../types/api';

const QualityAssuranceScreen: React.FC = () => {
  const [assessments, setAssessments] = useState<QualityAssessment[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [showCompleteModal, setShowCompleteModal] = useState(false);
  const [selectedAssessment, setSelectedAssessment] = useState<QualityAssessment | null>(null);
  const [completionData, setCompletionData] = useState<CompleteQualityAssessmentRequest>({
    score: 0,
    feedback: '',
    criteria: [],
  });

  const loadAssessments = async () => {
    try {
      const response = await apiService.getQualityAssessments();
      if (response.success) {
        setAssessments(response.data.data);
      } else {
        Alert.alert('Error', response.message || 'Failed to load quality assessments');
      }
    } catch (error) {
      console.error('Failed to load quality assessments:', error);
      setAssessments([
        {
          id: '1',
          aiRequestId: 'req1',
          assessorId: 'assessor1',
          assessorName: 'Quality Reviewer',
          type: 'manual',
          status: 'pending',
          createdAt: '2024-01-15T10:00:00Z',
          dueDate: '2024-01-20T10:00:00Z',
          criteria: [
            {
              id: 'c1',
              name: 'Accuracy',
              description: 'Content accuracy and correctness',
              weight: 0.4,
            },
            {
              id: 'c2',
              name: 'Relevance',
              description: 'Relevance to requirements',
              weight: 0.3,
            },
            {
              id: 'c3',
              name: 'Quality',
              description: 'Overall quality and completeness',
              weight: 0.3,
            },
          ],
        },
        {
          id: '2',
          aiRequestId: 'req2',
          assessorId: 'assessor2',
          assessorName: 'Senior Reviewer',
          type: 'expert-review',
          status: 'completed',
          score: 92,
          feedback: 'Excellent work with minor improvements needed',
          createdAt: '2024-01-14T09:00:00Z',
          completedAt: '2024-01-15T11:00:00Z',
          criteria: [
            {
              id: 'c1',
              name: 'Accuracy',
              description: 'Content accuracy and correctness',
              weight: 0.4,
              score: 95,
              comments: 'Very accurate',
            },
          ],
        },
      ]);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadAssessments();
  }, []);

  const onRefresh = () => {
    setRefreshing(true);
    loadAssessments();
  };

  const handleCompleteAssessment = (assessment: QualityAssessment) => {
    setSelectedAssessment(assessment);
    setCompletionData({
      score: 0,
      feedback: '',
      criteria: assessment.criteria.map(c => ({
        id: c.id,
        score: 0,
        comments: '',
      })),
    });
    setShowCompleteModal(true);
  };

  const submitCompletion = async () => {
    if (!selectedAssessment) return;

    if (completionData.score < 0 || completionData.score > 100) {
      Alert.alert('Error', 'Score must be between 0 and 100');
      return;
    }

    if (!completionData.feedback.trim()) {
      Alert.alert('Error', 'Please provide feedback');
      return;
    }

    try {
      const response = await apiService.completeQualityAssessment(
        selectedAssessment.id,
        completionData
      );
      if (response.success) {
        setShowCompleteModal(false);
        setSelectedAssessment(null);
        loadAssessments();
        Alert.alert('Success', 'Quality assessment completed successfully');
      } else {
        Alert.alert('Error', response.message || 'Failed to complete assessment');
      }
    } catch (error) {
      console.error('Failed to complete assessment:', error);
      Alert.alert('Error', 'Failed to complete assessment');
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'pending':
        return '#f59e0b';
      case 'in-progress':
        return '#3b82f6';
      case 'completed':
        return '#10b981';
      case 'overdue':
        return '#ef4444';
      default:
        return '#6b7280';
    }
  };

  const getTypeColor = (type: string) => {
    switch (type) {
      case 'automated':
        return '#8b5cf6';
      case 'manual':
        return '#3b82f6';
      case 'peer-review':
        return '#10b981';
      case 'expert-review':
        return '#f59e0b';
      default:
        return '#6b7280';
    }
  };

  const renderAssessment = ({ item }: { item: QualityAssessment }) => (
    <Card style={styles.assessmentCard}>
      <View style={styles.assessmentHeader}>
        <View style={styles.assessmentInfo}>
          <Text style={styles.assessmentTitle}>Assessment #{item.id}</Text>
          <Text style={styles.assessmentSubtitle}>AI Request: {item.aiRequestId}</Text>
          <Text style={styles.assessmentSubtitle}>Assessor: {item.assessorName}</Text>
        </View>
        <View style={styles.badges}>
          <View style={[styles.badge, { backgroundColor: getStatusColor(item.status) }]}>
            <Text style={styles.badgeText}>{item.status}</Text>
          </View>
          <View style={[styles.badge, { backgroundColor: getTypeColor(item.type) }]}>
            <Text style={styles.badgeText}>{item.type}</Text>
          </View>
        </View>
      </View>

      {item.score !== undefined && (
        <View style={styles.scoreContainer}>
          <Text style={styles.scoreLabel}>Score: {item.score}/100</Text>
          <View style={styles.scoreBar}>
            <View
              style={[styles.scoreFill, { width: `${item.score}%` }]}
            />
          </View>
        </View>
      )}

      {item.feedback && (
        <View style={styles.feedbackContainer}>
          <Text style={styles.feedbackLabel}>Feedback:</Text>
          <Text style={styles.feedbackText}>{item.feedback}</Text>
        </View>
      )}

      <View style={styles.assessmentDetails}>
        <Text style={styles.detailText}>
          Created: {new Date(item.createdAt).toLocaleDateString()}
        </Text>
        {item.dueDate && (
          <Text style={styles.detailText}>
            Due: {new Date(item.dueDate).toLocaleDateString()}
          </Text>
        )}
        {item.completedAt && (
          <Text style={styles.detailText}>
            Completed: {new Date(item.completedAt).toLocaleDateString()}
          </Text>
        )}
      </View>

      {item.status === 'pending' && (
        <TouchableOpacity
          style={styles.completeButton}
          onPress={() => handleCompleteAssessment(item)}
        >
          <Text style={styles.completeButtonText}>Complete Assessment</Text>
        </TouchableOpacity>
      )}
    </Card>
  );

  if (loading && assessments.length === 0) {
    return (
      <View style={styles.loadingContainer}>
        <Text style={styles.loadingText}>Loading quality assessments...</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>Quality Assurance</Text>
        <Text style={styles.subtitle}>Manage quality assessments and reviews</Text>
      </View>

      <FlatList
        data={assessments}
        renderItem={renderAssessment}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.listContainer}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
      />

      <Modal
        visible={showCompleteModal}
        animationType="slide"
        presentationStyle="pageSheet"
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalHeader}>
            <TouchableOpacity onPress={() => setShowCompleteModal(false)}>
              <Text style={styles.cancelButton}>Cancel</Text>
            </TouchableOpacity>
            <Text style={styles.modalTitle}>Complete Assessment</Text>
            <TouchableOpacity onPress={submitCompletion}>
              <Text style={styles.saveButton}>Submit</Text>
            </TouchableOpacity>
          </View>

          <View style={styles.modalContent}>
            <Text style={styles.inputLabel}>Overall Score (0-100)</Text>
            <TextInput
              style={styles.input}
              placeholder="Enter score"
              value={completionData.score.toString()}
              onChangeText={(text) => setCompletionData({ ...completionData, score: parseInt(text) || 0 })}
              keyboardType="numeric"
            />

            <Text style={styles.inputLabel}>Feedback</Text>
            <TextInput
              style={[styles.input, styles.textArea]}
              placeholder="Enter your feedback"
              value={completionData.feedback}
              onChangeText={(text) => setCompletionData({ ...completionData, feedback: text })}
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
  assessmentCard: {
    marginBottom: 16,
  },
  assessmentHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  assessmentInfo: {
    flex: 1,
  },
  assessmentTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#111827',
    marginBottom: 4,
  },
  assessmentSubtitle: {
    fontSize: 12,
    color: '#6b7280',
    marginBottom: 2,
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
  scoreContainer: {
    marginBottom: 12,
  },
  scoreLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#374151',
    marginBottom: 8,
  },
  scoreBar: {
    height: 8,
    backgroundColor: '#e5e7eb',
    borderRadius: 4,
    overflow: 'hidden',
  },
  scoreFill: {
    height: '100%',
    backgroundColor: '#10b981',
  },
  feedbackContainer: {
    marginBottom: 12,
  },
  feedbackLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#374151',
    marginBottom: 4,
  },
  feedbackText: {
    fontSize: 14,
    color: '#6b7280',
  },
  assessmentDetails: {
    marginBottom: 16,
    gap: 4,
  },
  detailText: {
    fontSize: 12,
    color: '#6b7280',
  },
  completeButton: {
    backgroundColor: '#2563eb',
    paddingVertical: 8,
    paddingHorizontal: 16,
    borderRadius: 6,
    alignSelf: 'flex-start',
  },
  completeButtonText: {
    fontSize: 14,
    fontWeight: '500',
    color: '#ffffff',
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
  inputLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#374151',
    marginBottom: 8,
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

export default QualityAssuranceScreen;
