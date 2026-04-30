import APIService from '../apiservice'
const apiUrl = 'Organization/Allocation'

class EmployeeAllocationService {

    // async getEmpWeekOffList(refId) {
    //     return await APIService.getAsync(`${apiUrl}/WeekOffSetups?refId=${refId}&page=0&size=10`)
    // }
    async getEmpMailList() {
        return await APIService.getAsync(`${apiUrl}/Mails`)
    }
    async searchEmpList(query, raiseById) {

        return await APIService.getAsync(`${apiUrl}/searchEmpList?name=${query}&&raiseById=${raiseById}`)
    }
    async UpdateAsync(data) {
            return await APIService.putAsync(`${apiUrl}/EmpWeekOffSetup`, data)
    }
    // async GetEmpAllocationById(id){
    //     return await APIService.getAsync(`${apiUrl}/${id}`)
    // }

    async GetEmpAllocationById(id){
        return await APIService.getAsync(`${apiUrl}/Employee/${id}`)
    }

    async DeleteWeekOffSetup(data){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffSetup`, data)
    }

    async getEmpWeekOffList(query) {
        return await APIService.getAsync(`${apiUrl}/WeekOffSetups?${query}`)
    }

}
export default new EmployeeAllocationService()