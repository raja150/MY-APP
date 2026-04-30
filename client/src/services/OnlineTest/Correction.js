import APIService from '../apiservice'
const apiUrl = 'OnlineTest/Correction'
class CorrectionService {

    async updateAsync(data) {
        if (data.id) {
            await APIService.putAsync(apiUrl, data);
        } else {
            await APIService.postAsync(apiUrl, data);
        }
    }

    async GetList() {
        return await APIService.getAsync(`${apiUrl}/List`);
    }

    async update(data) {
        await APIService.putAsync(`${apiUrl}/Correction`, data);
    }

    async GetQuestions(testId) {
        return await APIService.getAsync(`${apiUrl}/Question/${testId}`)
    }

    async GetAnswers(questionId) {
        return await APIService.getAsync(`${apiUrl}/Answer/${questionId}`)
    }

    paginate() {
        return {
            searchAPI: `${apiUrl}/Paginate`
        }
    }

}
export default new CorrectionService()