# PE Check - Student Attendance System

A modern Vue.js 3 web application for tracking student attendance in physical education lessons.

## Features

### Student Interface
- **Profile Page**: View personal information, current section, and attendance calendar
- **Sections**: Browse and search available PE sections with filtering options
- **Section Details**: View detailed information about sections including schedule and pricing
- **Normatives**: View and register for physical education normatives
- **QR Code**: Generate and display student QR code for attendance tracking

### Teacher Interface
- **Profile Page**: View personal information and assigned sections
- **Calendar**: Manage lessons and track student attendance
- **Section Management**: Edit section information, description, and schedule
- **Attendance Tracking**: Mark student attendance manually or via QR code scanning

### Moderator Interface
- **User Management**: Manage students, teachers, and sections
- **Data Administration**: Add, edit, and remove users and sections
- **System Oversight**: Monitor attendance and section enrollment

## Technology Stack

- **Frontend**: Vue.js 3 with TypeScript
- **State Management**: Pinia
- **Routing**: Vue Router 4
- **Styling**: Custom CSS with CSS Variables and Glassmorphism design
- **HTTP Client**: Axios
- **QR Code Generation**: qrcode library
- **Date Handling**: date-fns
- **Notifications**: vue3-toastify

## Design System

The application uses a modern design system with:
- **Primary Color**: #275886 (Blue)
- **Design Style**: Glassmorphism/Neomorphism with translucent UI elements
- **Responsive Design**: Mobile-first approach with adaptive layouts
- **Modern UI**: Rounded corners, shadows, and smooth transitions

## Project Structure

```
src/
├── api/                 # API client and service modules
│   ├── client.ts       # Axios configuration and interceptors
│   ├── auth.ts         # Authentication API
│   └── sections.ts     # Sections API
├── assets/             # Static assets and global styles
│   └── main.css        # Global CSS variables and utilities
├── components/         # Reusable Vue components
├── router/             # Vue Router configuration
│   └── index.ts        # Route definitions and guards
├── stores/             # Pinia stores
│   ├── auth.ts         # Authentication state management
│   └── sections.ts     # Sections state management
├── views/              # Page components
│   ├── LoginView.vue   # Authentication page
│   ├── StudentLayout.vue # Student dashboard layout
│   ├── TeacherLayout.vue # Teacher dashboard layout
│   ├── ModeratorLayout.vue # Moderator dashboard layout
│   ├── student/        # Student-specific views
│   ├── teacher/        # Teacher-specific views
│   └── moderator/      # Moderator-specific views
├── App.vue             # Root component
└── main.ts             # Application entry point
```

## Getting Started

### Prerequisites
- Node.js 18+ 
- npm or yarn

### Installation

1. Clone the repository
2. Navigate to the frontend directory:
   ```bash
   cd src/frontend/pecheck
   ```

3. Install dependencies:
   ```bash
   npm install
   ```

4. Start the development server:
   ```bash
   npm run dev
   ```

5. Open your browser and navigate to `http://localhost:3000`

### Building for Production

```bash
npm run build
```

The built files will be in the `dist/` directory.

## API Integration

The frontend is configured to work with an ASP.NET backend API. The API base URL is configured in `vite.config.ts` with a proxy to `http://localhost:5215`.

### Authentication
- JWT token-based authentication
- Automatic token storage in localStorage
- Request/response interceptors for error handling

### API Endpoints
- `/auth/login` - User authentication
- `/auth/me` - Get current user information
- `/sections` - CRUD operations for sections
- Additional endpoints for attendance, normatives, etc.

## Development

### Code Style
- TypeScript for type safety
- ESLint for code linting
- Prettier for code formatting

### Available Scripts
- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint
- `npm run format` - Format code with Prettier

## Responsive Design

The application is fully responsive with:
- **Desktop**: Full sidebar navigation
- **Tablet**: Collapsible sidebar
- **Mobile**: Bottom navigation bar

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing

1. Follow the existing code style and conventions
2. Use TypeScript for all new code
3. Write meaningful commit messages
4. Test your changes across different screen sizes

## License

This project is part of the PE Check student attendance system. 