/**
 * Error notification data
 */
interface ErrorNotification {
    status: boolean;
    label?: string;
    parameters?: { [key: string]: string };
}

export default ErrorNotification;
