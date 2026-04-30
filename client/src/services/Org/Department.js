import APIService from '../apiservice'
const apiUrl = 'Organization/Department'
class DepartmentService {

    async getDepSList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async paginate() {
        return await APIService.getAsync(`${apiUrl}/paginate`)
    }

    // async getDeptWeekOffList(refId) {
    //     return await APIService.getAsync(`${apiUrl}/WeekOffSetups?refId=${refId}&page=0&size=10`)
    // }
    
    async UpdateAsync(data) {
            return await APIService.putAsync(`${apiUrl}/DeptWeekOffSetup`, data)
    }
    async GetDeptAllocationById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async DeleteWeekOffSetup(data){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffSetup`, data)
    }
    async getDeptWeekOffLists(query) {
        return await APIService.getAsync(`${apiUrl}/WeekOffSetups?${query}`)
    }
}
export default new DepartmentService()