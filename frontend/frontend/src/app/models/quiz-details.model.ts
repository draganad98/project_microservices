export interface ChoiceDTO {
  id: number;
  questionId: number;
  text: string;
  isCorrect: boolean;
}

export interface QuestionFullDTO {
  id: number;
  quizId: number;
  type: string;
  points: number;
  text: string;
  correctText?: string | null;
  correct?: boolean | null;
  choices: ChoiceDTO[];
}

export interface QuizDetailsDTO {
  id: number;
  title: string;
  description: string;
  difficultyLevel: string;
  timeLimitSeconds: number;
  categoryIds: number[];
  questions: QuestionFullDTO[];
}
