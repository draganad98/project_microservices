export interface GetQuizzes {
  id: number;
  title: string;
  difficultyLevel: string;
  description?: string;
  timeLimitSeconds: number;
  categoryIds: number[]; 
  questionsNum: number;
}