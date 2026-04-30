import APIService from '../apiservice';
const apiUrl = 'Leave/Approval/CompensatoryDay';
class CompensatoryDayService{

    paginate(){
        return { searchAPI: `${apiUrl}` };
    }
    async updateAsync(data){
        if(data.id){
            return await APIService.putAsync(`${apiUrl}`, data)
        }
    }
    async GetApplyCompensatoryById(rid) {
        return await APIService.getAsync(`SelfService/ApplyCompensatoryWorkingDay/${rid}`)
    }
    async GetEmployees(){
        return await APIService.getAsync(`Organization/Employee/GetList`)
    }
}
export default new CompensatoryDayService();