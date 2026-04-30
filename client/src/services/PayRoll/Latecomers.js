import APIService from '../apiservice'
const apiUrl = 'PayRoll/Latecomers'
class LatecomersService {
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
    async PayMonths() {
        return await APIService.getAsync(`${apiUrl}/PayMonths`)
    }
}
export default new LatecomersService()