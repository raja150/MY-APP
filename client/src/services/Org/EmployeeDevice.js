import apiservice from "services/apiservice"
const apiUrl = 'Organization/EmployeeDevice'

class EmployeeDeviceService {
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await apiservice.getAsync(`${apiUrl}/${id}`)
    }

    async PostAsync(data) {
        if (data.id) {
            return await apiservice.putAsync(`${apiUrl}`, data)
        } else {
            return await apiservice.postAsync(`${apiUrl}`, data)
        }
    }
}
export default new EmployeeDeviceService()