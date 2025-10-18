import { Question } from "./question.model";

export interface CreateQuiz {
  creatorId: number;
  title: string;
  description: string;
  timeLimitSeconds: number;
  difficultyLevel: string;
  categoryIds: number[];
  questions: Question[]; 
}