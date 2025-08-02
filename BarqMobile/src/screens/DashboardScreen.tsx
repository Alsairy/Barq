import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  RefreshControl,
  Alert,
} from 'react-native';
import { Card } from '../components/Card';
import { StatCard } from '../components/StatCard';
import { apiService } from '../services/api';
import { DashboardStats } from '../types/api';

const DashboardScreen: React.FC = () => {
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  const getStatusBadgeStyle = (status: string) => {
    switch (status) {
      case 'approved':
        return { backgroundColor: '#10b981' };
      case 'pending':
        return { backgroundColor: '#f59e0b' };
      case 'in-progress':
        return { backgroundColor: '#3b82f6' };
      case 'completed':
        return { backgroundColor: '#6b7280' };
      default:
        return { backgroundColor: '#6b7280' };
    }
  };

  const getPriorityBadgeStyle = (priority: string) => {
    switch (priority) {
      case 'high':
        return { backgroundColor: '#ef4444' };
      case 'normal':
        return { backgroundColor: '#6b7280' };
      case 'low':
        return { backgroundColor: '#10b981' };
      default:
        return { backgroundColor: '#6b7280' };
    }
  };

  const loadDashboardStats = async () => {
    try {
      const response = await apiService.getDashboardStats();
      if (response.success) {
        setStats(response.data);
      } else {
        Alert.alert('Error', response.message || 'Failed to load dashboard stats');
      }
    } catch (error) {
      console.error('Failed to load dashboard stats:', error);
      setStats({
        aiRequests: {
          total: 156,
          pending: 23,
          approved: 98,
          rejected: 12,
          inProgress: 23,
        },
        workflows: {
          active: 45,
          completed: 234,
          failed: 8,
        },
        qualityAssessments: {
          pending: 15,
          completed: 89,
          averageScore: 87.5,
        },
      });
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadDashboardStats();
  }, []);

  const onRefresh = () => {
    setRefreshing(true);
    loadDashboardStats();
  };

  if (loading && !stats) {
    return (
      <View style={styles.loadingContainer}>
        <Text style={styles.loadingText}>Loading dashboard...</Text>
      </View>
    );
  }

  return (
    <ScrollView
      style={styles.container}
      refreshControl={
        <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
      }
    >
      <View style={styles.header}>
        <Text style={styles.title}>Dashboard</Text>
        <Text style={styles.subtitle}>
          Overview of your AI request processing and workflow management
        </Text>
      </View>

      <View style={styles.statsGrid}>
        <StatCard
          title="Total AI Requests"
          value={stats?.aiRequests.total.toString() || '0'}
          subtitle="+12% from last month"
          icon="smart-toy"
        />
        <StatCard
          title="Active Workflows"
          value={stats?.workflows.active.toString() || '0'}
          subtitle="+5 new this week"
          icon="account-tree"
        />
        <StatCard
          title="Quality Score"
          value={`${stats?.qualityAssessments.averageScore || 0}%`}
          subtitle="+2.5% improvement"
          icon="verified"
        />
        <StatCard
          title="Pending Reviews"
          value={stats?.aiRequests.pending.toString() || '0'}
          subtitle="Requires attention"
          icon="schedule"
        />
      </View>

      <Card style={styles.recentActivity}>
        <Text style={styles.cardTitle}>Recent AI Requests</Text>
        <View style={styles.activityList}>
          {[
            { title: 'Content Generation for Marketing', status: 'approved', priority: 'high' },
            { title: 'Data Analysis Report', status: 'pending', priority: 'normal' },
            { title: 'Code Review Assistant', status: 'in-progress', priority: 'high' },
            { title: 'Translation Service', status: 'completed', priority: 'low' },
          ].map((request, index) => (
            <View key={index} style={styles.activityItem}>
              <Text style={styles.activityTitle}>{request.title}</Text>
              <View style={styles.badges}>
                <View style={[styles.badge, getStatusBadgeStyle(request.status)]}>
                  <Text style={styles.badgeText}>{request.status}</Text>
                </View>
                <View style={[styles.badge, getPriorityBadgeStyle(request.priority)]}>
                  <Text style={styles.badgeText}>{request.priority}</Text>
                </View>
              </View>
            </View>
          ))}
        </View>
      </Card>

      <Card style={styles.systemAlerts}>
        <Text style={styles.cardTitle}>System Alerts</Text>
        <View style={styles.alertsList}>
          <View style={styles.alertItem}>
            <Text style={styles.alertTitle}>SLA Warning</Text>
            <Text style={styles.alertSubtitle}>3 requests approaching deadline</Text>
          </View>
          <View style={styles.alertItem}>
            <Text style={styles.alertTitle}>Quality Milestone</Text>
            <Text style={styles.alertSubtitle}>Average score exceeded 85%</Text>
          </View>
          <View style={styles.alertItem}>
            <Text style={styles.alertTitle}>Performance Update</Text>
            <Text style={styles.alertSubtitle}>Processing time improved by 15%</Text>
          </View>
        </View>
      </Card>
    </ScrollView>
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
  statsGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    paddingHorizontal: 10,
    marginBottom: 20,
  },
  recentActivity: {
    margin: 20,
    marginTop: 0,
  },
  systemAlerts: {
    margin: 20,
    marginTop: 0,
    marginBottom: 40,
  },
  cardTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#111827',
    marginBottom: 16,
  },
  activityList: {
    gap: 12,
  },
  activityItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 8,
  },
  activityTitle: {
    fontSize: 14,
    fontWeight: '500',
    color: '#111827',
    flex: 1,
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
  approvedBadge: {
    backgroundColor: '#10b981',
  },
  pendingBadge: {
    backgroundColor: '#f59e0b',
  },
  'in-progressBadge': {
    backgroundColor: '#3b82f6',
  },
  completedBadge: {
    backgroundColor: '#6b7280',
  },
  highBadge: {
    backgroundColor: '#ef4444',
  },
  normalBadge: {
    backgroundColor: '#6b7280',
  },
  lowBadge: {
    backgroundColor: '#10b981',
  },
  alertsList: {
    gap: 16,
  },
  alertItem: {
    paddingVertical: 4,
  },
  alertTitle: {
    fontSize: 14,
    fontWeight: '500',
    color: '#111827',
    marginBottom: 4,
  },
  alertSubtitle: {
    fontSize: 12,
    color: '#6b7280',
  },
});

export default DashboardScreen;
