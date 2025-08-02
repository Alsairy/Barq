import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { StatusBar, useColorScheme } from 'react-native';
// import Icon from 'react-native-vector-icons/MaterialIcons';

import DashboardScreen from './src/screens/DashboardScreen';
import AIRequestsScreen from './src/screens/AIRequestsScreen';
import WorkflowsScreen from './src/screens/WorkflowsScreen';
import QualityAssuranceScreen from './src/screens/QualityAssuranceScreen';

const Tab = createBottomTabNavigator();

function App() {
  const isDarkMode = useColorScheme() === 'dark';

  return (
    <NavigationContainer>
      <StatusBar barStyle={isDarkMode ? 'light-content' : 'dark-content'} />
      <Tab.Navigator
        screenOptions={({ route }) => ({
          tabBarIcon: ({ focused, color, size }) => {
            let iconName: string;

            switch (route.name) {
              case 'Dashboard':
                iconName = 'dashboard';
                break;
              case 'AI Requests':
                iconName = 'smart-toy';
                break;
              case 'Workflows':
                iconName = 'account-tree';
                break;
              case 'Quality Assurance':
                iconName = 'verified';
                break;
              default:
                iconName = 'help';
            }

            return null;
          },
          tabBarActiveTintColor: '#2563eb',
          tabBarInactiveTintColor: 'gray',
          headerStyle: {
            backgroundColor: isDarkMode ? '#1f2937' : '#ffffff',
          },
          headerTintColor: isDarkMode ? '#ffffff' : '#000000',
          tabBarStyle: {
            backgroundColor: isDarkMode ? '#1f2937' : '#ffffff',
          },
        })}
      >
        <Tab.Screen name="Dashboard" component={DashboardScreen} />
        <Tab.Screen name="AI Requests" component={AIRequestsScreen} />
        <Tab.Screen name="Workflows" component={WorkflowsScreen} />
        <Tab.Screen name="Quality Assurance" component={QualityAssuranceScreen} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}

export default App;
