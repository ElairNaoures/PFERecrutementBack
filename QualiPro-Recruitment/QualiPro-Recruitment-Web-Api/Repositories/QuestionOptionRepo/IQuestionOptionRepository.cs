using QualiPro_Recruitment_Data.Models;

public interface IQuestionOptionRepository
{
    Task<IEnumerable<TabQuestionOption>> GetAllQuestionOptions();
    Task<TabQuestionOption> GetQuestionOptionById(int id);
    Task<IEnumerable<TabQuestionOption>> GetAllQuestionOptionsByQuestionId(int questionId); // Ajouté
    Task<TabQuestionOption> AddQuestionOption(TabQuestionOption questionOption);
    Task<TabQuestionOption> UpdateQuestionOption(int id, TabQuestionOption questionOption);
    Task<bool> DeleteQuestionOption(int id);
  

}
