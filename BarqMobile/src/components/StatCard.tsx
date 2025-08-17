import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from './Card';

interface StatCardProps {
  title: string;
  value: string;
  subtitle: string;
  icon: string;
}

export const StatCard: React.FC<StatCardProps> = ({ title, value, subtitle }) => {
  return (
    <View style={styles.container}>
      <Card style={styles.card}>
        <Text style={styles.title}>{title}</Text>
        <Text style={styles.value}>{value}</Text>
        <Text style={styles.subtitle}>{subtitle}</Text>
      </Card>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    width: '50%',
    padding: 10,
  },
  card: {
    padding: 16,
  },
  title: {
    fontSize: 14,
    fontWeight: '500',
    color: '#6b7280',
    marginBottom: 8,
  },
  value: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#111827',
    marginBottom: 4,
  },
  subtitle: {
    fontSize: 12,
    color: '#6b7280',
  },
});
