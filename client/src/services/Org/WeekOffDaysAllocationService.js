import APIService from '../apiservice'
const apiUrl = 'Organization/WeekOffDays'

class WeekOffDaysAllocationService {

    // async getWeekOffDaysList(refId) {
    //     return await APIService.getAsync(`${apiUrl}/WeekOffSetups?refId=${refId}&page=0&size=10`)
    // }
    
    async UpdateAsync(data) {
            return await APIService.putAsync(`${apiUrl}/WeekOffDaysSetup`, data)
    }
    async GetWeekOffAllocationById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getWeekOffDaysList(refId){
        return await APIService.getAsync(`Leave/WeekOffDays/Paginate?refId=${refId}&page=0&size=10`)
    }

    async DeleteWeekOffSetup(data){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffSetup`, data)
    }
}
export default new WeekOffDaysAllocationService()