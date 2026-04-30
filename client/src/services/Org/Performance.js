import apiservice from 'services/apiservice'

const apiUrl = 'Organization/Performance';

class PerformanceService {
    async postAsync(data) {
        if (data.id) {
            return await apiservice.putAsync(`${apiUrl}`, data)
        } else {
            return await apiservice.postAsync(`${apiUrl}`, data)
        }
    }
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await apiservice.getAsync(`${apiUrl}/${id}`)
    }
}
export default new PerformanceService()