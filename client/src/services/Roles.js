import APIService from './apiservice'
const apiUrl = 'Role'

class RoleService {
    paginate() { 
        return {
            searchAPI: `${apiUrl}/Paginate`
        }
    }
    async getAllRoles() {
        return await APIService.getAsync(`${apiUrl}/All`)
    }
}
export default new RoleService()