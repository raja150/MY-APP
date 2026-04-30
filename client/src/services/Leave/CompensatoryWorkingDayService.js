import APIService from '../apiservice';
const apiUrl = 'Leave/ApplyCompensatoryWorkingDay'

class CompensatoryWorkingDayService{

    paginate(){
        return{searchAPI :`${apiUrl}/Paginate` }
    }
    SelfServicePaginate(){
        return{searchAPI :`${apiUrl}/Self` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}`, data)
        }
    }
}
export default new CompensatoryWorkingDayService();

