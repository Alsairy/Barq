# BARQ UI Component Library

A comprehensive design system built with React, TypeScript, and Tailwind CSS, featuring 50+ reusable components with WCAG 2.1 AA accessibility compliance.

## Design Tokens

### Colors
- **Primary**: Blue scale (50-950) for main actions and branding
- **Secondary**: Gray scale (50-950) for text and backgrounds  
- **Success**: Green scale for positive actions and feedback
- **Warning**: Yellow scale for caution and alerts
- **Error**: Red scale for errors and destructive actions

### Typography
- **Font Family**: Inter (system fallback)
- **Font Sizes**: 12px-96px with consistent line heights
- **Font Weights**: 300-900 for proper hierarchy

### Spacing
- **Scale**: 4px base unit (0.25rem) up to 384px (24rem)
- **Consistent**: All components use the same spacing scale

## Core Components

### Layout Components
- **Card**: Container with shadow and rounded corners
- **Separator**: Visual divider between content sections
- **Skeleton**: Loading placeholder with animation

### Form Components
- **Button**: Primary, secondary, outline, ghost variants
- **Input**: Text input with validation states
- **Textarea**: Multi-line text input
- **Checkbox**: Boolean selection with indeterminate state
- **Label**: Accessible form labels
- **Select**: Dropdown selection component

### Navigation Components
- **Breadcrumb**: Hierarchical navigation trail
- **Pagination**: Page navigation controls
- **Tabs**: Content organization and switching

### Feedback Components
- **Alert**: Status messages and notifications
- **Badge**: Status indicators and labels
- **Progress**: Task completion indicators
- **Toast**: Temporary notifications

### Data Display
- **Avatar**: User profile images with fallbacks
- **DataTable**: Sortable, filterable data tables
- **Table**: Basic table structure

### Overlay Components
- **Dialog**: Modal dialogs and confirmations
- **Popover**: Contextual information overlays
- **Tooltip**: Hover information displays
- **Sheet**: Slide-out panels

## Accessibility Features

### WCAG 2.1 AA Compliance
- **Keyboard Navigation**: Full keyboard support for all interactive elements
- **Screen Reader Support**: Proper ARIA labels and descriptions
- **Focus Management**: Visible focus indicators and logical tab order
- **Color Contrast**: Minimum 4.5:1 contrast ratio for text
- **Text Scaling**: Supports up to 200% zoom without horizontal scrolling

### Accessibility Utilities
- **VisuallyHidden**: Screen reader only content
- **FocusVisible**: Enhanced focus indicators
- **useKeyboardNavigation**: Keyboard event handling
- **useFocusTrap**: Focus containment for modals
- **useAnnouncement**: Screen reader announcements

## Theme System

### Light/Dark Mode Support
- **ThemeProvider**: Context-based theme management
- **ThemeToggle**: User theme selection component
- **CSS Variables**: Dynamic color switching
- **System Preference**: Automatic theme detection

### Customization
- **Design Tokens**: Centralized styling variables
- **CSS Custom Properties**: Runtime theme switching
- **Tailwind Configuration**: Extended color palette and utilities

## Animation System

### Micro-interactions
- **Hover Effects**: Subtle state changes
- **Focus Animations**: Smooth focus transitions
- **Loading States**: Spinner and skeleton animations
- **Page Transitions**: Smooth navigation

### Performance
- **CSS Transforms**: Hardware-accelerated animations
- **Reduced Motion**: Respects user preferences
- **Optimized Timing**: Natural easing functions

## Usage Examples

### Basic Button
```tsx
import { Button } from '@/components/ui/button';

<Button variant="primary" size="md">
  Click me
</Button>
```

### Form with Validation
```tsx
import { Input, Label, Button } from '@/components/ui';

<form>
  <Label htmlFor="email">Email</Label>
  <Input 
    id="email" 
    type="email" 
    placeholder="Enter your email"
    required 
  />
  <Button type="submit">Submit</Button>
</form>
```

### Data Table
```tsx
import { DataTable } from '@/components/ui/data-table';

const columns = [
  { accessorKey: 'name', header: 'Name' },
  { accessorKey: 'email', header: 'Email' },
];

<DataTable 
  columns={columns} 
  data={users} 
  searchKey="name"
  searchPlaceholder="Search users..."
/>
```

### Theme Toggle
```tsx
import { ThemeToggle } from '@/components/ui/theme-toggle';

<ThemeToggle />
```

## Development Workflow

### Code Quality
- **ESLint**: Consistent code style and error detection
- **Prettier**: Automatic code formatting
- **TypeScript**: Type safety and better developer experience

### Testing
- **Jest**: Unit testing framework
- **React Testing Library**: Component testing utilities
- **Accessibility Testing**: Automated a11y checks

### Build Process
- **Vite**: Fast development and optimized production builds
- **Tailwind CSS**: Utility-first styling with purging
- **Tree Shaking**: Optimized bundle sizes

## Browser Support

- **Modern Browsers**: Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
- **Mobile**: iOS Safari 14+, Chrome Mobile 90+
- **Accessibility**: Screen readers and assistive technologies

## Performance

- **Bundle Size**: Optimized with tree shaking and code splitting
- **Runtime Performance**: Minimal re-renders and efficient updates
- **Loading**: Lazy loading and progressive enhancement
- **Caching**: Proper cache headers and versioning
