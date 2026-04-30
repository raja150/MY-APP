import APIService from '../apiservice';
const apiUrl = 'Organization/Designation'
class DesignationService{
    async getDesignationsList(){
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }


    // async getDesignWeekOffList(refId) {
    //     return await APIService.getAsync(`${apiUrl}/WeekOffSetups?refId=${refId}&page=0&size=10`)
    // }
    
    async UpdateAsync(data) {
            return await APIService.putAsync(`${apiUrl}/DesignWeekOffSetup`, data)
    }
    async GetDesignAllocationById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async DeleteWeekOffSetup(data){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffSetup`, data)
    }

    async getDesignWeekOffList(query) {
        return await APIService.getAsync(`${apiUrl}/WeekOffSetups?${query}`)
    }

}
export default new DesignationService();