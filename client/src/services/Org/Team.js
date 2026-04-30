import APIService from '../apiservice';
const apiUrl = 'Organization/Team'
class TeamService{
    async getLocationsList(){
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }


    // async getTeamWeekOffList(refId) {
    //     return await APIService.getAsync(`${apiUrl}/WeekOffSetups?refId=${refId}&page=0&size=10`)
    // }
    
    async UpdateAsync(data) {
            return await APIService.putAsync(`${apiUrl}/TeamWeekOffSetup`, data)
    }
    async GetTeamAllocationById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async DeleteWeekOffSetup(data){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffSetup`, data)
    }

    async getTeamWeekOffList(query) {
        return await APIService.getAsync(`${apiUrl}/WeekOffSetups?${query}`)
    }
}
export default new TeamService();