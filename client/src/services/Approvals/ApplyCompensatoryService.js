import APIService from '../apiservice';
const apiUrl = 'Approval/ApprovalCompensatoryWorkingDay';
class ApplyCompensatoryService{

    paginate(){
        return { searchAPI: `${apiUrl}` };
    }
    async updateAsync(data){
        if(data.id){
            return await APIService.putAsync('Approval/ApprovalCompensatoryWorkingDay', data)
        }
    }
    async GetApplyCompensatoryById(rid) {
        return await APIService.getAsync(`SelfService/ApplyCompensatoryWorkingDay/${rid}`)
    }
    async GetEmployees(){
        return await APIService.getAsync(`Organization/Employee/GetList`)
    }
}
export default new ApplyCompensatoryService();