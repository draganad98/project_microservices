import { Component, EventEmitter, Output } from '@angular/core';
import { Question, Choice } from '../../models/question.model';

@Component({
  selector: 'app-add-question',
  templateUrl: './add-question.component.html',
  styleUrls: ['./add-question.component.css']
})
export class AddQuestionComponent {
  @Output() done = new EventEmitter<Question>();
  @Output() close = new EventEmitter<void>();

  question: Question = this.getEmptyQuestion();

  getEmptyQuestion(): Question {
    return {
      type: 'fillBlank',
      text: '',
      points: 1,
      correctText: '',
      correct: undefined,
      choices: [
        { text: '', isCorrect: false },
        { text: '', isCorrect: false },
        { text: '', isCorrect: false },
        { text: '', isCorrect: false }
      ],
      quizId:-1
    };
  }

  doneClicked() {
    if (!this.question.text || !this.question.points) {
      alert('Fill in the text and points!');
      return;
    }

    if (this.question.type === 'fillBlank' && !this.question.correctText) {
      alert('Enter the correct answer!');
      return;
    }

    if (this.question.type === 'trueFalse' && this.question.correct === undefined) {
      alert('Select True or False!');
      return;
    }

    if (this.question.type === 'multipleOne') {
      const filledChoices = this.question.choices?.filter(c => c.text.trim() !== '');
      if (!filledChoices || filledChoices.length < 4) {
        alert('Enter the options!');
        return;
      }
      if (!this.question.choices?.some(c => c.isCorrect)) {
        alert('Select one correct option!');
        return;
      }
    }

    if (this.question.type === 'multipleMore') {
      const filledChoices = this.question.choices?.filter(c => c.text.trim() !== '');
      if (!filledChoices || filledChoices.length < 4) {
        alert('Enter the options!');
        return;
      }
      if (!this.question.choices?.some(c => c.isCorrect)) {
        alert('Select one correct option!');
        return;
      }
    }
    if (this.question.type === 'fillBlank' || this.question.type === 'trueFalse') {
      this.question.choices = []; 
  }
    this.done.emit(this.question);
    this.question = this.getEmptyQuestion();
  }

  closeClicked() {
    this.close.emit();
  }

  selectCorrect(index: number) {
    if (!this.question.choices) return;
    this.question.choices.forEach((c, i) => c.isCorrect = i === index);
  }
}


