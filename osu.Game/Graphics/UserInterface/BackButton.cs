﻿// Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Transformations;
using osu.Framework.Input;

namespace osu.Game.Graphics.UserInterface
{
    // Basic back button as it was on stable (kinda). No skinning possible for now
    public class BackButton : ClickableContainer
    {
        private TextAwesome icon;

        private Container leftContainer;
        private Container rightContainer;

        private Box leftBox;
        private Box rightBox;

        private const double transform_time = 300.0;
        private const int pulse_length = 250;

        private const float shear = 0.1f;

        private static readonly Vector2 size_extended = new Vector2(140, 50);
        private static readonly Vector2 size_retracted = new Vector2(100, 50);

        public BackButton()
        {
            Size = size_retracted;

            Children = new Drawable[]
            {
                leftContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.4f,
                    Children = new Drawable[]
                    {
                        leftBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Color4(195, 40, 140, 255),
                            Shear = new Vector2(shear, 0),
                        },
                        icon = new TextAwesome
                        {
                            Anchor = Anchor.Centre,
                            TextSize = 25,
                            Icon = FontAwesome.fa_osu_left_o
                        },
                    }
                },
                rightContainer = new Container
                {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.6f,
                    Children = new Drawable[]
                    {
                        rightBox = new Box
                        {
                            Colour = new Color4(238, 51, 153, 255),
                            Origin = Anchor.TopLeft,
                            Anchor = Anchor.TopLeft,
                            RelativeSizeAxes = Axes.Both,
                            Shear = new Vector2(shear, 0),
                            EdgeSmoothness = new Vector2(1.5f, 0),
                        },
                        new SpriteText
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Text = @"Back",
                        }
                    }
                }
            };
        }

        public override bool Contains(Vector2 screenSpacePos)
        {
            return leftBox.Contains(screenSpacePos) || rightBox.Contains(screenSpacePos);
        }

        protected override bool OnHover(InputState state)
        {
            icon.ClearTransformations();

            ResizeTo(size_extended, transform_time, EasingTypes.OutElastic);

            int duration = 0; //(int)(Game.Audio.BeatLength / 2);
            if (duration == 0) duration = pulse_length;

            double offset = 0; //(1 - Game.Audio.SyncBeatProgress) * duration;
            double startTime = Time.Current + offset;

            // basic pulse
            icon.Transforms.Add(new TransformScale
            {
                StartValue = new Vector2(1.1f),
                EndValue = Vector2.One,
                StartTime = startTime,
                EndTime = startTime + duration,
                Easing = EasingTypes.Out,
                LoopCount = -1,
                LoopDelay = duration
            });

            return true;
        }

        protected override void OnHoverLost(InputState state)
        {
            icon.ClearTransformations();

            ResizeTo(size_retracted, transform_time, EasingTypes.OutElastic);

            int duration = 0; //(int)(Game.Audio.BeatLength);
            if (duration == 0) duration = pulse_length * 2;

            double offset = 0; //(1 - Game.Audio.SyncBeatProgress) * duration;
            double startTime = Time.Current + offset;

            // slow pulse
            icon.Transforms.Add(new TransformScale
            {
                StartValue = new Vector2(1.1f),
                EndValue = Vector2.One,
                StartTime = startTime,
                EndTime = startTime + duration,
                Easing = EasingTypes.Out,
                LoopCount = -1,
                LoopDelay = duration
            });
        }

        protected override bool OnClick(InputState state)
        {
            var flash = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Shear = new Vector2(shear, 0),
                Colour = new Color4(255, 255, 255, 128),
            };
            Add(flash);

            flash.FadeOutFromOne(200);
            flash.Expire();

            return base.OnClick(state);
        }
    }
}
