import APIService from '../apiservice';
const apiUrl='Leave/Approval/WFH'
class WFHService{
    paginate(){
        return {searchAPI:`${apiUrl}`}
    }
    async getById(id){
        return await APIService.getAsync(`SelfService/ApplyWFH/${id}`);
    }
    async getEmp(userId){
        
        return await APIService.getAsync(`Organization/Employee/Emps/${userId}`);
    }
    async GetAllEmp(){
        return await APIService.getAsync(`Organization/Employee/GetList`);
    }
    async updateAsync(data){
        return await APIService.putAsync(`Leave/Approval/Wfh`, data);
    }
}
export default new WFHService();