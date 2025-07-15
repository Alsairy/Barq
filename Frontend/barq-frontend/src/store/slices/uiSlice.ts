import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UiState {
  theme: 'light' | 'dark';
  sidebarCollapsed: boolean;
  notifications: Array<{
    id: string;
    type: 'success' | 'error' | 'warning' | 'info';
    title: string;
    message: string;
    timestamp: number;
  }>;
  loading: {
    global: boolean;
    [key: string]: boolean;
  };
}

const initialState: UiState = {
  theme: (localStorage.getItem('theme') as 'light' | 'dark') || 'light',
  sidebarCollapsed: localStorage.getItem('sidebarCollapsed') === 'true',
  notifications: [],
  loading: {
    global: false,
  },
};

const uiSlice = createSlice({
  name: 'ui',
  initialState,
  reducers: {
    setTheme: (state, action: PayloadAction<'light' | 'dark'>) => {
      state.theme = action.payload;
      localStorage.setItem('theme', action.payload);
    },
    toggleSidebar: (state) => {
      state.sidebarCollapsed = !state.sidebarCollapsed;
      localStorage.setItem('sidebarCollapsed', state.sidebarCollapsed.toString());
    },
    addNotification: (state, action: PayloadAction<Omit<UiState['notifications'][0], 'id' | 'timestamp'>>) => {
      const notification = {
        ...action.payload,
        id: Date.now().toString(),
        timestamp: Date.now(),
      };
      state.notifications.push(notification);
    },
    removeNotification: (state, action: PayloadAction<string>) => {
      state.notifications = state.notifications.filter(n => n.id !== action.payload);
    },
    setLoading: (state, action: PayloadAction<{ key: string; loading: boolean }>) => {
      state.loading[action.payload.key] = action.payload.loading;
    },
  },
});

export const { setTheme, toggleSidebar, addNotification, removeNotification, setLoading } = uiSlice.actions;
export default uiSlice.reducer;
