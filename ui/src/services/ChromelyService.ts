/**
 * Send a query to backend
 *
 * @param request Request
 * @param onSuccess Callback on success
 * @param onError Callback on error
 */
export function query(request: object, onSuccess: (response: string) => void, onError: (error: Error, message: string) => void) {
    return (window as any).cefQuery({
        request: JSON.stringify(request),
        onSuccess: onSuccess,
        onFailure: onError
    });
}
