export interface GetAdminQuizzes {
  id: number;
  title: string;
  description?: string;
  questionsCount: number;
  categories: string[]; 
}