import APIService from '../apiservice'
const apiUrl = 'Leave/WeekOffDays'
class WeekOffDaysService1 {

    async getWeekOffDaysList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async paginate() {
        return await APIService.getAsync(`${apiUrl}/paginate`)
    }

    async getWeekOffDaysList(refId) {
        return await APIService.getAsync(`${apiUrl}/WeekOffDays?refId=${refId}&page=0&size=10`)
    }
    
    async UpdateAsync(data) {
        if(data.id){
            return await APIService.putAsync(`Leave/WeekOffDays`, data)
        }
            return await APIService.postAsync(`Leave/WeekOffDays`, data)
    }
    async GetWeekOffDaysById(id){
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async DeleteWeekOffDays(id){
        return await APIService.putAsync(`${apiUrl}/DeleteWeekOffDays/${id}`, {})
    }

    async getWeekOffDaysLists(query) {
        return await APIService.getAsync(`${apiUrl}/WeekOffDays?${query}`)
    }
  
}
export default new WeekOffDaysService1()