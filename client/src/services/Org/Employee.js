import APIService from '../apiservice'
const apiUrl = 'Organization/Employee'
class EmployeeService {
    paginate(){
        return { searchAPI: `${apiUrl}/Paginate`};
    }
    async getEmpById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getEmpSList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async getEmpMailList() {
        return await APIService.getAsync(`${apiUrl}/Mails`)
    }
    async searchEmpList(query,raiseById){
      
        return await APIService.getAsync(`${apiUrl}/searchEmpList?name=${query}&&raiseById=${raiseById}`)
    }
    async GetLeaveTypes(){
        return await APIService.getAsync(`${apiUrl}/LeaveType`)
    }
    async GetLeavesBasedOnLeaveTypes(){
        return await APIService.getAsync(`${apiUrl}/LeavesBasedOnLeaveType`)
    }
    async getEmployeeById(id) {
        return await APIService.getAsync(`${apiUrl}/Image/${id}`)
    }
}
export default new EmployeeService()