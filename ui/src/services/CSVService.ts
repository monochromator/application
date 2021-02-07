const EOL = "\n";
const DELIMITER = ";";
const HEADERS = [ "STEPS", "RAW" ];

/**
 * Convert analysis data into a CSV string
 *
 * @param data Analysis data
 * @return {string} CSV string
 */
const toCSV = (data: [number, number][]): string => {
    let output = "";

    // Write headers
    output += HEADERS.join(DELIMITER) + EOL;

    // Write values
    for (const row of data) {
        output += row.join(DELIMITER) + EOL;
    }

    return output;
};

export { toCSV };
