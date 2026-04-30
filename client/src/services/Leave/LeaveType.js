import APIService from '../apiservice'
const apiUrl = 'Leave/LeaveType'
const apiUrl1 = 'Leave/LeaveAccumulation'
class LeaveTypeService {
    async getLeaveType() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async getEmpLeaves(empId, leaveTypeId,attDate) {
        return await APIService.getAsync(`${apiUrl1}/EmpLeaves/${empId}/${leaveTypeId}/${attDate}`)
    }
    async getLeaveTypeById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getDefaultPayOffLeaveType(){
        return await APIService.getAsync(`${apiUrl}/DefaultPayOffType`)
    }
    async getPaidLeaveTypes(){
        return await APIService.getAsync(`${apiUrl}/GetPaidLeaveTypes`)
    }
}
export default new LeaveTypeService()