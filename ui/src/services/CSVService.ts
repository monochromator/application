const EOL = "\n";
const DELIMITER = ";";
const HEADERS = [ "STEPS", "RAW" ];

type HeaderToIndexMap = {
    [key: string]: number | undefined;
};

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

/**
 * Extract analysis data from the given CSV
 *
 * @param csv CSV string
 * @return {} Analysis data
 */
const fromCSV = (csv: string): [number, number][] => {
    const data: [number, number][] = [];

    // Parse csv
    const csvAsArray = csv.split(EOL).map((row: string) => row.split(DELIMITER));

    // No headers, skip parsing
    if (csvAsArray.length === 0) {
        return data;
    }

    // Parse headers
    const headerToIndex = HEADERS
        .map((header: string): [number, string] => [ csvAsArray[0].indexOf(header), header ])
        .reduce((map: HeaderToIndexMap, indexAndHeader: [number, string]) => {
            map[indexAndHeader[1]] = indexAndHeader[0] >= 0 ? indexAndHeader[0] : undefined;
            return map;
        }, {} as HeaderToIndexMap);

    // Create data
    for (let i = 1; i < csvAsArray.length; i++) {
        const row = HEADERS.map((header: string) => {
            const index = headerToIndex[header];

            if (index === undefined) {
                return undefined;
            }
            const value = parseFloat(csvAsArray[i][index]);
            if (Number.isNaN(value)) {
                return undefined;
            }
            return value;
        });

        if (!row.includes(undefined)) {
            data.push(row as [number, number]);
        }
    }

    return data;
};

export { toCSV, fromCSV };
