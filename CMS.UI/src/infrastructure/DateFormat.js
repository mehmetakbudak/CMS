export class DateFormat {

    convert(value) {
        return new Date(value).toLocaleDateString("tr-TR", {
            day: "2-digit",
            month: "2-digit",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit",
            second: "2-digit",
        });
    }

    convertShortDate(value) {
        return new Date(value).toLocaleDateString("tr-TR", {
            day: "2-digit",
            month: "2-digit",
            year: "numeric"
        });
    }
}

export default new DateFormat();