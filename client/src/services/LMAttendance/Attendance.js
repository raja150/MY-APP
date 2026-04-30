import APIService from '../apiservice'

const apiUrl = 'LM_Attendance/Attendance'
class AttendanceService {

    async DownloadAttendance(date) {
        return await APIService.getBlobAsync(`${apiUrl}/Download/${date}`)
    }
    async getAttendanceData(qString) {
        return await APIService.getAsync(`${apiUrl}/GetAttendanceData?${qString}`)
    }
    async UpdateAttendance(data) {
        return await APIService.putAsync(`${apiUrl}/UpdateAttendance`, data);
    }
    async getIsPunchInAsync() {
        return await APIService.getAsync(`${apiUrl}/IsPunchIn`);
    }
    async putAsync() {
        return await APIService.putAsync(`${apiUrl}/Time`);
    }
    async postAsync(data) {
        return await APIService.postAsync(`${apiUrl}/Time`, data);
    }
    async putRePunchIn(data) {
        return await APIService.putAsync(`${apiUrl}/RePunchIn`, data);
    }
    async isPunchedEmployee() {
        return await APIService.getAsync(`${apiUrl}/IsPunchedEmployee`);
    }
    async Finalized(date){
        return await APIService.getAsync(`${apiUrl}/Finalized/${date}`)
    }
    async getMovementTimings(attendanceDate,loginType){
        return await APIService.getAsync(`${apiUrl}/Movement/${attendanceDate}/${loginType}`)
    }
}
export default new AttendanceService()