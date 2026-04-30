import APIService from '../apiservice'
const apiUrl = 'Leave/WeekOffSetup'
class WeekOffSetupService {
    
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }

    async getList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}`, data)
        }
    }
    
}
export default new WeekOffSetupService()