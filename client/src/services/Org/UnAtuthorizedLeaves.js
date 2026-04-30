import apiservice from '../apiservice'
const apiUrl = 'LM/UnAuthorizedLeaves'
class UnAuthorizedLeavesService {

    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getUnAuthorizedLeavesById(id) {
        return await apiservice.getAsync(`${apiUrl}/${id}`)
    }
    async AddUnAuthorizedLeaves(data) {
        if (data.id) {
            return await apiservice.putAsync(`${apiUrl}`, data)
        } else {
            return await apiservice.postAsync(`${apiUrl}`, data)
        }
    }
}
export default new UnAuthorizedLeavesService()