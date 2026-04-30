import APIService from '../apiservice';
const apiUrl = 'Organization/Location'
class LocationService{

    async getLocationsList(){
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async GetLocationsById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }


    async UpdateAsync(data) {
            return await APIService.putAsync(`${apiUrl}/LocationWeekOffSetup`, data)
    }
    async GetLocAllocationById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }

    async DeleteWeekOffSetup(data){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffSetup`, data)
    }
    async getLocWeekOffList(query) {
        return await APIService.getAsync(`${apiUrl}/WeekOffSetups?${query}`)
    }
     // async getLocWeekOffList(refId) {
    //     return await APIService.getAsync(`${apiUrl}/WeekOffSetups?refId=${refId}&page=0&size=10`)
    // }
}
export default new LocationService();