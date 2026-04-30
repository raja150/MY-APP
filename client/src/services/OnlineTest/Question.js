import APIService from '../apiservice'
const apiUrl = 'OnlineTest/Question'
class QuestionService {

    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        }
        return await APIService.postAsync(`${apiUrl}`, data)
    }
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async GetById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async GetQuestions() {
        return await APIService.getAsync(`${apiUrl}`)
    }
    async deleteQuestion(id) {
        return await APIService.putAsync(`${apiUrl}/delete?id=${id}`,{})
    }


}
export default new QuestionService()