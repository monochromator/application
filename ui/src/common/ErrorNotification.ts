/**
 * Error notification data
 */
export default interface ErrorNotification {
    status: boolean;
    label?: string;
    parameters?: { [key: string]: string };
};
