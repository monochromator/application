/**
 * Response returned by query function
 */
interface CefResponse {
    Data: object;
    ReadyState: number;
    Status: number;
    StatusText: string;
}

/**
 * Error
 */
interface Error {
    Data: object;
    ReadyState: number;
    RequestId: string;
    Status: number;
    StatusText: string;
}

/**
 * HTTP methods
 */
enum HttpMethod {
    Get = "GET",
    Post = "POST"
}

/**
 * Request sent by query function
 */
interface CefRequest {
    method: HttpMethod;
    url: string;
    parameters?: { [key: string]: string };
    postData?: object;
}

/**
 * Send a query to backend
 *
 * @param request Request
 * @param onSuccess Callback on success
 * @param onError Callback on error
 */
const query = function(request: CefRequest, onSuccess: (response: CefResponse) => void, onError: (error: Error) => void) {
    return (window as any).cefQuery({
        request: JSON.stringify(request),
        onSuccess: function(response: string) {
            // Parse response
            const responseAsJson = JSON.parse(response);

            // Call callbacks based on response
            if (responseAsJson.Status === 200) {
                onSuccess(responseAsJson as CefResponse);
            } else {
                onError(responseAsJson as Error);
            }
        }
    });
};

export { HttpMethod, CefRequest, CefResponse, query };
