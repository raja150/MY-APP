import APIservice from "services/apiservice";
const apiUrl = 'SelfService/Compliance'
class ComplianceService {
    async post(data) {
        return await APIservice.postFormDataAsync(`${apiUrl}`, data)
    }
    async isUploaded() {
        return await APIservice.getBlobAsync(`${apiUrl}`)
    }
}
export default new ComplianceService()