import * as moment from "moment";

export function serializeDateOnly(value: string | undefined | null): string | undefined {
    if (!value) {
        return undefined;
    }

    return moment(value).format('YYYY-MM-DD');
}
