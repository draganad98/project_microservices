export interface Choice {
  text: string;
  isCorrect: boolean;
}

export interface Question {
  id?: number;
  type: 'fillBlank' | 'trueFalse' | 'multipleOne' | 'multipleMore';
  text: string;
  points: number;
  correctText?: string; 
  correct?: boolean;    
  choices?: Choice[];   
  quizId: number;
}



