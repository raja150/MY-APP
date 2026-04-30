import APIService from '../apiservice'
const apiUrl = 'PayRoll/EmployeePayInfo'
class PaymentInfoService {
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}`, data)
        }
    }
}
export default new PaymentInfoService()