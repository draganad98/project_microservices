import { Question } from "./question.model";

export interface UpdateQuiz {
  title: string;
  description: string;
  timeLimitSeconds: number;
  difficultyLevel: string;
  categoryIds: number[];
  questions: Question[]; 
  questionsNum?: number;
}