import APIService from './apiservice'
const apiUrl = 'User'
const apiUrl1 = 'Organization/Employee'


class UserService {
    paginate() { 
        return {
            searchAPI: `${apiUrl}/Paginate`
        }
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}`, data)
        }
    }
    async getUserById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getEmpById(id){
        return await APIService.getAsync(`${apiUrl1}/${id}`)
    }

    async getUserAudit(rid) {
        return await APIService.getAsync(`${apiUrl}/History/${rid}`)
    }
    
    async ResetAsync(data) { 
        return await APIService.putAsync(`${apiUrl}/reset`, data)
    }
}
export default new UserService();