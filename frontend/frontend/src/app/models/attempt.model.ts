export interface CreateAttempt {
  quizId: number;
  playerId: number | null;
  startedAt: string;
  finishedAt: string;
}

export interface AttemptResponse {
  id: number;
  quizId: number;
  playerId: number;
  startedAt: string;
  finishedAt: string;
  correctAnsNum?: number;             
  correctAnsPercentage?: number;
  score: number;
  totalQuestions: number; 
  durationSeconds: number;
}

export interface CreateUserAnswer {
  attemptId: number;
  questionId: number;
  textAnswer?: string;
  correct?: boolean | null;
  choiceIds?: number[];
}

export interface Attempt {
  id: number;
  startedAt: string;         
  score: number;             
}

export interface AttemptRankingDTO {
  position: number;
  username: string;
  score: number;
  finishedAt: string; 
  picture: string;
}
