import {
    useToast
} from "primevue/usetoast";

class AlertService {

    successMessage(message) {
        const toast = useToast();
        toast.add({
            severity: "success",
            summary: "İşlem Başarılı",
            detail: message,
            life: 5000
        });
    }

    errorMessage(message) {
        const toast = useToast();
        toast.add({
            severity: "error",
            summary: "İşlem Başarısız",
            detail: message,
            life: 5000
        });
    }

}
export default new AlertService();