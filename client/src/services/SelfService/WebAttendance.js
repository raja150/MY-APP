import APIService from '../apiservice'
const apiUrl = 'WebAttendance'
class WebAttendanceService{
    async putAsync() {
        return await APIService.putAsync(`${apiUrl}`);
    }
    async postAsync(data) {
        return await APIService.postAsync(`${apiUrl}`, data);
    }
    async RePunchIn(data) {
        return await APIService.putAsync(`${apiUrl}/RePunchIn`, data);
    }
    async getIsPunchInAsync() {
        return await APIService.getAsync(`${apiUrl}/IsPunchIn`);
    }
    async isPunchedEmployee() {
        return await APIService.getAsync(`${apiUrl}/IsPunchedEmployee`);
    }
    async putRePunchIn(data) {
        return await APIService.putAsync(`${apiUrl}/RePunchIn`, data);
    }


    ApprovalPaginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async gerById(id){
        return await APIService.getAsync(`${apiUrl}/Approval/${id}`)
    }
    async Approve(data){
        return await APIService.putAsync(`${apiUrl}/Approval`,data)
    }
}
export default new WebAttendanceService()