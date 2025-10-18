export interface Question {
  id?: number;
  quizId?: number;
  type: string;
  text: string;
  points: number;
}

export interface QuizWithQuestions {
  id: number;
  title: string;
  description: string;
  difficultyLevel: string;
  timeLimitSeconds: number;
  categoryIds: number[];
  questions: Question[];
}
